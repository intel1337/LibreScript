<script>
    import { onMount } from 'svelte';
    import { goto } from '$app/navigation';
    import { currentUser, isLoggedIn } from '$lib/stores/authStore.js';
    import { getUserPosts, updateProfile, updatePostStatus, deletePost } from '$lib/services/profileService.js';
    import { getLanguageIcon, getLanguageLabel } from '$lib/data/languages.js';
    import { updateAuthState } from '$lib/stores/authStore.js';
    import { getCurrentUser } from '$lib/services/loginService.js';
    import "../../app.css";

    let loading = false;
    let error = null;
    let success = null;
    let userPosts = [];
    let postsLoading = false;
    let postsError = null;

    // Profile form data
    let profileForm = {
        fullName: '',
        email: '',
        password: '',
        confirmPassword: ''
    };

    // Active tab
    let activeTab = 'profile';

    // Post management
    let editingStatus = {};
    let newStatuses = {};

    onMount(async () => {
        // Check if user is logged in
        if (!$isLoggedIn || !$currentUser) {
            goto('/login');
            return;
        }

        // Initialize profile form with current user data
        profileForm.fullName = $currentUser.fullName || '';
        profileForm.email = $currentUser.email || '';

        // Load user posts
        await loadUserPosts();
    });

    async function loadUserPosts() {
        postsLoading = true;
        postsError = null;

        console.log('Loading user posts...');
        console.log('Current user:', $currentUser);
        console.log('Current user ID:', $currentUser?.userId);
        console.log('Token in localStorage:', localStorage.getItem('token'));

        try {
            userPosts = await getUserPosts();
            console.log('User posts loaded:', userPosts);
        } catch (err) {
            console.error('Error loading user posts:', err);
            postsError = err.message;
        } finally {
            postsLoading = false;
        }
    }

    async function handleProfileUpdate() {
        if (profileForm.password && profileForm.password !== profileForm.confirmPassword) {
            error = 'Passwords do not match';
            return;
        }

        loading = true;
        error = null;
        success = null;

        try {
            const updateData = {
                fullName: profileForm.fullName,
                email: profileForm.email
            };

            // Only include password if it's provided
            if (profileForm.password) {
                updateData.password = profileForm.password;
            }

            await updateProfile(updateData);
            success = 'Profile updated successfully!';
            
            // Refresh user data
            const updatedUser = await getCurrentUser();
            updateAuthState(true, updatedUser);
            
            // Clear password fields
            profileForm.password = '';
            profileForm.confirmPassword = '';
        } catch (err) {
            console.error('Error updating profile:', err);
            error = err.message;
        } finally {
            loading = false;
        }
    }

    async function handleStatusUpdate(postId, newStatus) {
        try {
            await updatePostStatus(postId, newStatus);
            
            // Update local state
            userPosts = userPosts.map(post => 
                post.id === postId ? { ...post, status: newStatus } : post
            );
            
            editingStatus[postId] = false;
            editingStatus = { ...editingStatus };
        } catch (err) {
            console.error('Error updating post status:', err);
            alert('Error updating post status: ' + err.message);
        }
    }

    async function handleDeletePost(postId) {
        if (!confirm('Are you sure you want to delete this post? This action cannot be undone.')) {
            return;
        }

        try {
            await deletePost(postId);
            
            // Remove from local state
            userPosts = userPosts.filter(post => post.id !== postId);
        } catch (err) {
            console.error('Error deleting post:', err);
            alert('Error deleting post: ' + err.message);
        }
    }

    function startEditingStatus(postId, currentStatus) {
        editingStatus[postId] = true;
        newStatuses[postId] = currentStatus;
        editingStatus = { ...editingStatus };
        newStatuses = { ...newStatuses };
    }

    function cancelEditingStatus(postId) {
        editingStatus[postId] = false;
        delete newStatuses[postId];
        editingStatus = { ...editingStatus };
        newStatuses = { ...newStatuses };
    }

    function formatDate(dateString) {
        return new Date(dateString).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    }

    function navigateToPost(postId) {
        goto(`/post/${postId}`);
    }

    const statusOptions = ['Open', 'Pending', 'In Progress', 'Solved', 'Closed'];
</script>

<svelte:head>
    <title>Profile - LibreScript</title>
</svelte:head>

<div class="profile-container">
    <div class="profile-header">
        <h1>My Profile</h1>
        <p>Welcome back, {$currentUser?.username || 'User'}!</p>
    </div>

    <div class="tabs">
        <button 
            class="tab" 
            class:active={activeTab === 'profile'}
            on:click={() => activeTab = 'profile'}
        >
            Profile Settings
        </button>
        <button 
            class="tab" 
            class:active={activeTab === 'posts'}
            on:click={() => activeTab = 'posts'}
        >
            My Posts ({userPosts.length})
        </button>
    </div>

    {#if activeTab === 'profile'}
        <div class="profile-section">
            <h2>Profile Information</h2>
            
            {#if error}
                <div class="error-message">{error}</div>
            {/if}
            
            {#if success}
                <div class="success-message">{success}</div>
            {/if}

            <form on:submit|preventDefault={handleProfileUpdate}>
                <div class="form-group">
                    <label for="username">Username</label>
                    <input 
                        type="text" 
                        id="username" 
                        value={$currentUser?.username || ''} 
                        disabled
                        class="form-input disabled"
                    />
                    <small>Username cannot be changed</small>
                </div>

                <div class="form-group">
                    <label for="fullName">Full Name</label>
                    <input 
                        type="text" 
                        id="fullName" 
                        bind:value={profileForm.fullName}
                        class="form-input"
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="email">Email</label>
                    <input 
                        type="email" 
                        id="email" 
                        bind:value={profileForm.email}
                        class="form-input"
                        required
                    />
                </div>

                <div class="form-group">
                    <label for="password">New Password (optional)</label>
                    <input 
                        type="password" 
                        id="password" 
                        bind:value={profileForm.password}
                        class="form-input"
                        placeholder="Leave blank to keep current password"
                    />
                </div>

                <div class="form-group">
                    <label for="confirmPassword">Confirm New Password</label>
                    <input 
                        type="password" 
                        id="confirmPassword" 
                        bind:value={profileForm.confirmPassword}
                        class="form-input"
                        placeholder="Confirm new password"
                        disabled={!profileForm.password}
                    />
                </div>

                <button type="submit" class="update-btn" disabled={loading}>
                    {loading ? 'Updating...' : 'Update Profile'}
                </button>
            </form>
        </div>
    {:else if activeTab === 'posts'}
        <div class="posts-section">
            <h2>My Posts</h2>
            
            {#if postsLoading}
                <div class="loading">Loading your posts...</div>
            {:else if postsError}
                <div class="error-message">
                    Error loading posts: {postsError}
                    <br><small>Check the browser console for more details</small>
                </div>
            {:else if userPosts.length === 0}
                <div class="empty-state">
                    <p>You haven't posted any questions yet.</p>
                    <a href="/new-post" class="new-post-btn">Ask Your First Question</a>
                </div>
            {:else}
                <div class="posts-list">
                    {#each userPosts as post (post.id)}
                        <div class="post-card">
                            <div class="post-header">
                                <h3 class="post-title" on:click={() => navigateToPost(post.id)}>
                                    {post.title}
                                </h3>
                                <div class="post-meta">
                                    <span class="language">
                                        <span class="lang-icon">{getLanguageIcon(post.language)}</span>
                                        {getLanguageLabel(post.language)}
                                    </span>
                                    <span class="date">{formatDate(post.createdAt)}</span>
                                </div>
                            </div>
                            
                            <div class="post-content">
                                <p>{post.content.substring(0, 200)}{post.content.length > 200 ? '...' : ''}</p>
                            </div>
                            
                            <div class="post-stats">
                                <span class="stat">
                                    <strong>{post.upvotes}</strong> upvotes
                                </span>
                                <span class="stat">
                                    <strong>{post.downvotes}</strong> downvotes
                                </span>
                                <span class="stat">
                                    <strong>{post.commentsCount}</strong> replies
                                </span>
                                <span class="stat">
                                    <strong>{post.views}</strong> views
                                </span>
                            </div>
                            
                            <div class="post-actions">
                                <div class="status-section">
                                    {#if editingStatus[post.id]}
                                        <select bind:value={newStatuses[post.id]} class="status-select">
                                            {#each statusOptions as status}
                                                <option value={status}>{status}</option>
                                            {/each}
                                        </select>
                                        <button 
                                            class="save-btn"
                                            on:click={() => handleStatusUpdate(post.id, newStatuses[post.id])}
                                        >
                                            Save
                                        </button>
                                        <button 
                                            class="cancel-btn"
                                            on:click={() => cancelEditingStatus(post.id)}
                                        >
                                            Cancel
                                        </button>
                                    {:else}
                                        <span class="status {post.status === 'Solved' ? 'solved' : 'needhelp'}">
                                            {post.status}
                                        </span>
                                        <button 
                                            class="edit-status-btn"
                                            on:click={() => startEditingStatus(post.id, post.status)}
                                        >
                                            Edit Status
                                        </button>
                                    {/if}
                                </div>
                                
                                <div class="action-buttons">
                                    <button 
                                        class="view-btn"
                                        on:click={() => navigateToPost(post.id)}
                                    >
                                        View
                                    </button>
                                    <button 
                                        class="delete-btn"
                                        on:click={() => handleDeletePost(post.id)}
                                    >
                                        Delete
                                    </button>
                                </div>
                            </div>
                        </div>
                    {/each}
                </div>
            {/if}
        </div>
    {/if}
</div>

<style>
    .profile-container {
        max-width: 1000px;
        margin: 20px auto;
        padding: 20px;
        font-family: "Inter", Arial, sans-serif;
    }

    .profile-header {
        text-align: center;
        margin-bottom: 30px;
    }

    .profile-header h1 {
        font-size: 2.5rem;
        color: #24292f;
        margin-bottom: 8px;
    }

    .profile-header p {
        color: #656d76;
        font-size: 1.1rem;
    }

    .tabs {
        display: flex;
        gap: 4px;
        margin-bottom: 30px;
        border-bottom: 2px solid #e1e5e9;
    }

    .tab {
        padding: 12px 24px;
        background: none;
        border: none;
        font-size: 1rem;
        font-weight: 500;
        color: #656d76;
        cursor: pointer;
        border-bottom: 2px solid transparent;
        transition: all 0.3s ease;
    }

    .tab:hover {
        color: #0969da;
        background: #f6f8fa;
    }

    .tab.active {
        color: #0969da;
        border-bottom-color: #0969da;
        background: #f6f8fa;
    }

    .profile-section, .posts-section {
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        padding: 30px;
    }

    .profile-section h2, .posts-section h2 {
        margin-bottom: 20px;
        color: #24292f;
    }

    .form-group {
        margin-bottom: 20px;
    }

    .form-group label {
        display: block;
        margin-bottom: 6px;
        font-weight: 500;
        color: #24292f;
    }

    .form-input {
        width: 100%;
        padding: 12px 16px;
        border: 2px solid #e1e5e9;
        border-radius: 8px;
        font-size: 1rem;
        transition: border-color 0.3s ease;
        box-sizing: border-box;
    }

    .form-input:focus {
        outline: none;
        border-color: #0969da;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.1);
    }

    .form-input.disabled {
        background: #f6f8fa;
        color: #656d76;
        cursor: not-allowed;
    }

    .form-group small {
        color: #656d76;
        font-size: 0.85rem;
        margin-top: 4px;
        display: block;
    }

    .update-btn {
        background: linear-gradient(135deg, #0969da 0%, #0550ae 100%);
        color: white;
        border: none;
        padding: 12px 24px;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.3s ease;
    }

    .update-btn:hover:not(:disabled) {
        background: linear-gradient(135deg, #0550ae 0%, #0969da 100%);
        transform: translateY(-1px);
        box-shadow: 0 4px 12px rgba(9, 105, 218, 0.3);
    }

    .update-btn:disabled {
        opacity: 0.6;
        cursor: not-allowed;
        transform: none;
    }

    .error-message {
        background: #ffeaea;
        color: #d73a49;
        padding: 12px 16px;
        border-radius: 8px;
        margin-bottom: 20px;
        border: 1px solid #f1c0c0;
    }

    .success-message {
        background: #e6f7ec;
        color: #1a7f37;
        padding: 12px 16px;
        border-radius: 8px;
        margin-bottom: 20px;
        border: 1px solid #c0f0c0;
    }

    .loading {
        text-align: center;
        padding: 40px;
        color: #656d76;
        font-size: 1.1rem;
    }

    .empty-state {
        text-align: center;
        padding: 60px 20px;
    }

    .empty-state p {
        color: #656d76;
        font-size: 1.1rem;
        margin-bottom: 20px;
    }

    .new-post-btn {
        display: inline-block;
        background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
        color: white;
        text-decoration: none;
        padding: 12px 24px;
        border-radius: 8px;
        font-weight: 600;
        transition: all 0.3s ease;
    }

    .new-post-btn:hover {
        transform: translateY(-1px);
        box-shadow: 0 4px 12px rgba(40, 167, 69, 0.3);
    }

    .posts-list {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }

    .post-card {
        border: 2px solid #e1e5e9;
        border-radius: 12px;
        padding: 20px;
        transition: all 0.3s ease;
    }

    .post-card:hover {
        border-color: #0969da;
        box-shadow: 0 4px 12px rgba(9, 105, 218, 0.1);
    }

    .post-header {
        margin-bottom: 12px;
    }

    .post-title {
        font-size: 1.2rem;
        font-weight: 600;
        color: #0969da;
        margin-bottom: 8px;
        cursor: pointer;
        transition: color 0.3s ease;
    }

    .post-title:hover {
        color: #0550ae;
        text-decoration: underline;
    }

    .post-meta {
        display: flex;
        gap: 16px;
        align-items: center;
        font-size: 0.9rem;
        color: #656d76;
    }

    .language {
        display: flex;
        align-items: center;
        gap: 4px;
    }

    .post-content {
        margin-bottom: 16px;
        color: #24292f;
        line-height: 1.5;
    }

    .post-stats {
        display: flex;
        gap: 20px;
        margin-bottom: 16px;
        font-size: 0.9rem;
    }

    .stat {
        color: #656d76;
    }

    .stat strong {
        color: #24292f;
    }

    .post-actions {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding-top: 16px;
        border-top: 1px solid #e1e5e9;
    }

    .status-section {
        display: flex;
        align-items: center;
        gap: 8px;
    }

    .status {
        padding: 4px 12px;
        border-radius: 6px;
        font-size: 0.85rem;
        font-weight: 600;
    }

    .status.solved {
        background: #e6f7ec;
        color: #1a7f37;
    }

    .status.needhelp {
        background: #fff3e6;
        color: #b26a00;
    }

    .status-select {
        padding: 4px 8px;
        border: 1px solid #e1e5e9;
        border-radius: 4px;
        font-size: 0.85rem;
    }

    .edit-status-btn, .save-btn, .cancel-btn, .view-btn, .delete-btn {
        padding: 6px 12px;
        border: 1px solid #e1e5e9;
        border-radius: 4px;
        font-size: 0.85rem;
        cursor: pointer;
        transition: all 0.3s ease;
    }

    .edit-status-btn, .view-btn {
        background: #f6f8fa;
        color: #24292f;
    }

    .edit-status-btn:hover, .view-btn:hover {
        background: #e1e5e9;
    }

    .save-btn {
        background: #28a745;
        color: white;
        border-color: #28a745;
    }

    .save-btn:hover {
        background: #20c997;
    }

    .cancel-btn {
        background: #6c757d;
        color: white;
        border-color: #6c757d;
    }

    .cancel-btn:hover {
        background: #5a6268;
    }

    .delete-btn {
        background: #dc3545;
        color: white;
        border-color: #dc3545;
    }

    .delete-btn:hover {
        background: #c82333;
    }

    .action-buttons {
        display: flex;
        gap: 8px;
    }

    @media (max-width: 768px) {
        .profile-container {
            padding: 10px;
        }

        .profile-section, .posts-section {
            padding: 20px;
        }

        .tabs {
            flex-direction: column;
        }

        .tab {
            text-align: left;
            border-bottom: 1px solid #e1e5e9;
            border-radius: 0;
        }

        .tab.active {
            border-bottom-color: #0969da;
        }

        .post-actions {
            flex-direction: column;
            align-items: stretch;
            gap: 12px;
        }

        .status-section {
            justify-content: center;
        }

        .action-buttons {
            justify-content: center;
        }

        .post-stats {
            flex-wrap: wrap;
            gap: 12px;
        }

        .post-meta {
            flex-direction: column;
            align-items: flex-start;
            gap: 8px;
        }
    }
</style>
