<script>
  import { onMount } from 'svelte';

  const API_BASE_URL = 'http://localhost:5028/api'; // IMPORTANT: Adjust if your backend port or base path is different

  // --- Reactive variables to store fetched data ---
  let allPosts = [];
  let allCategories = [];
  let allComments = []; // Will store all comments, potentially including replies if the API returns them flat

  let userById = null;
  let userIdInput = '';

  let userByUsername = null;
  let usernameInput = '';

  let postById = null;
  let postIdInput = '';

  let categoryById = null;
  let categoryIdInput = '';

  let commentById = null;
  let commentIdInput = '';

  let repliesForComment = [];
  let parentCommentIdInput = '';

  let error = null; // To display any fetch errors
  let isLoadingInitialData = true; // Changed: Specific flag for initial load
  let isLoadingAction = false;    // New: Flag for individual button actions

  // --- Generic Fetch Function ---
  async function fetchData(url, errorMessagePrefix) {
    // isLoading will be handled by the calling context (initial vs action)
    error = null;
    try {
      const response = await fetch(url);
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`${errorMessagePrefix}: ${response.status} ${response.statusText}. Backend: ${errorText}`);
      }
      const data = await response.json();
      console.log(`[SUCCESS] Fetched ${url}:`, data); // Log successful data
      return data;
    } catch (e) {
      console.error(`[ERROR] Fetching ${url}:`, e);
      error = e.message;
      return null;
    }
  }

  // --- Fetch All (onMount) ---
  onMount(async () => {
    isLoadingInitialData = true;
    console.log('[onMount] Starting to fetch initial data...');
    try {
      const postsPromise = fetchData(`${API_BASE_URL}/Post`, 'Error fetching all posts');
      const categoriesPromise = fetchData(`${API_BASE_URL}/Category`, 'Error fetching all categories');
      const commentsPromise = fetchData(`${API_BASE_URL}/Comment`, 'Error fetching all comments');

      // Wait for all initial fetches to complete
      const [postsResult, categoriesResult, commentsResult] = await Promise.all([
        postsPromise,
        categoriesPromise,
        commentsPromise
      ]);

      // Access the actual array from the $values property
      allPosts = (postsResult && postsResult.$values) ? postsResult.$values : [];
      allCategories = (categoriesResult && categoriesResult.$values) ? categoriesResult.$values : [];
      allComments = (commentsResult && commentsResult.$values) ? commentsResult.$values : [];

      console.log('[onMount] Initial data assigned (after extracting $values):');
      console.log('[onMount] allPosts:', allPosts);
      console.log('[onMount] allCategories:', allCategories);
      console.log('[onMount] allComments:', allComments);

    } catch (e) {
      // This catch might be redundant if fetchData handles its own errors and sets the 'error' variable
      // but kept for safety for Promise.all rejection.
      console.error('[onMount] Error during initial data fetch sequence:', e);
      // error variable should already be set by a failing fetchData call
    } finally {
      isLoadingInitialData = false;
      console.log('[onMount] Finished fetching initial data. isLoadingInitialData:', isLoadingInitialData);
    }
  });

  // --- User Fetch Functions ---
  async function handleFetchUserById() {
    if (!userIdInput.trim()) return;
    isLoadingAction = true;
    userById = await fetchData(`${API_BASE_URL}/User/get-user/id/${userIdInput.trim()}`, `Error fetching user by ID ${userIdInput.trim()}`);
    isLoadingAction = false;
  }

  async function handleFetchUserByUsername() {
    if (!usernameInput.trim()) return;
    isLoadingAction = true;
    userByUsername = await fetchData(`${API_BASE_URL}/User/get-user/username/${usernameInput.trim()}`, `Error fetching user by username ${usernameInput.trim()}`);
    isLoadingAction = false;
  }

  // --- Post Fetch Functions ---
  async function handleFetchPostById() {
    if (postIdInput === '') return;
    isLoadingAction = true;
    postById = await fetchData(`${API_BASE_URL}/Post/${postIdInput}`, `Error fetching post by ID ${postIdInput}`);
    isLoadingAction = false;
  }

  // --- Category Fetch Functions ---
  async function handleFetchCategoryById() {
    if (categoryIdInput === '') return;
    isLoadingAction = true;
    categoryById = await fetchData(`${API_BASE_URL}/Category/${categoryIdInput}`, `Error fetching category by ID ${categoryIdInput}`);
    isLoadingAction = false;
  }

  // --- Comment Fetch Functions ---
  async function handleFetchCommentById() {
    if (commentIdInput === '') return;
    isLoadingAction = true;
    commentById = await fetchData(`${API_BASE_URL}/Comment/${commentIdInput}`, `Error fetching comment by ID ${commentIdInput}`);
    isLoadingAction = false;
  }

  async function handleFetchRepliesForComment() {
    if (parentCommentIdInput === '') return;
    isLoadingAction = true;
    repliesForComment = await fetchData(`${API_BASE_URL}/Comment/${parentCommentIdInput}/replies`, `Error fetching replies for comment ID ${parentCommentIdInput}`) || [];
    isLoadingAction = false;
  }

</script>

<svelte:head>
  <title>LibreScript Data Viewer</title>
</svelte:head>

<style>
  :global(body) { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif; margin: 0; background-color: #f4f4f9; color: #333; }
  .page-container { max-width: 1200px; margin: 20px auto; padding: 20px; background-color: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
  h1 { text-align: center; color: #2c3e50; margin-bottom: 30px; }
  h2 { color: #34495e; border-bottom: 2px solid #7f8c8d; padding-bottom: 10px; margin-top: 30px; }
  .section-container { margin-bottom: 40px; padding: 20px; border: 1px solid #e0e0e0; border-radius: 6px; background-color: #fdfdfd; }
  .input-group { display: flex; gap: 10px; margin-bottom: 15px; align-items: center; }
  .input-group label { font-weight: 500; }
  .input-group input[type="text"], .input-group input[type="number"] { padding: 8px 12px; border: 1px solid #ccc; border-radius: 4px; flex-grow: 1; min-width: 100px; }
  .input-group button { padding: 8px 15px; background-color: #3498db; color: white; border: none; border-radius: 4px; cursor: pointer; transition: background-color 0.2s; }
  .input-group button:hover { background-color: #2980b9; }
  .data-display { background-color: #ecf0f1; padding: 15px; border-radius: 4px; margin-top:10px; max-height: 300px; overflow-y: auto; border: 1px solid #bdc3c7; }
  .error-message { color: #c0392b; font-weight: bold; margin: 20px 0; padding: 15px; border: 1px solid #e74c3c; background-color: #fbeae5; border-radius: 4px; white-space: pre-wrap; }
  .loading-message { text-align: center; padding: 20px; font-style: italic; color: #7f8c8d; }
  ul { list-style-type: none; padding-left: 0; }
  li { background-color: #fff; border: 1px solid #e0e0e0; padding: 10px; margin-bottom: 8px; border-radius: 4px; }
  pre { white-space: pre-wrap; word-wrap: break-word; font-size: 0.9em; }
  /* hr { border: 0; border-top: 1px solid #eee; margin: 20px 0; } */ /* Removed as it was unused */
</style>

<div class="page-container">
  <h1>LibreScript API Data Viewer</h1>

  {#if isLoadingInitialData || isLoadingAction}
    <p class="loading-message">Loading data...</p>
  {/if}

  {#if error && !(isLoadingInitialData || isLoadingAction) }
    <div class="error-message">
      <p>An error occurred:</p>
      <pre>{error}</pre>
    </div>
  {/if}

  <!-- Users Section -->
  <div class="section-container">
    <h2>Users</h2>
    <div class="input-group">
      <label for="userId">Fetch User by ID:</label>
      <input id="userId" type="number" bind:value={userIdInput} placeholder="Enter User ID" />
      <button on:click={handleFetchUserById} disabled={isLoadingInitialData || isLoadingAction || userIdInput === ''}>Fetch User</button>
    </div>
    {#if userById}
      <div class="data-display">
        <h3>User Details (ID: {userIdInput})</h3>
        <pre>{JSON.stringify(userById, null, 2)}</pre>
      </div>
    {/if}

    <div class="input-group">
      <label for="username">Fetch User by Username:</label>
      <input id="username" type="text" bind:value={usernameInput} placeholder="Enter Username" />
      <button on:click={handleFetchUserByUsername} disabled={isLoadingInitialData || isLoadingAction || !usernameInput.trim()}>Fetch User</button>
    </div>
    {#if userByUsername}
      <div class="data-display">
        <h3>User Details (Username: {usernameInput})</h3>
        <!-- The get-user/username endpoint might return a string directly or a JSON object -->
        <pre>{typeof userByUsername === 'object' ? JSON.stringify(userByUsername, null, 2) : userByUsername}</pre>
      </div>
    {/if}
  </div>

  <!-- Posts Section -->
  <div class="section-container">
    <h2>Posts</h2>
    <div class="input-group">
      <label for="postId">Fetch Post by ID:</label>
      <input id="postId" type="number" bind:value={postIdInput} placeholder="Enter Post ID" />
      <button on:click={handleFetchPostById} disabled={isLoadingInitialData || isLoadingAction || postIdInput === ''}>Fetch Post</button>
    </div>
    {#if postById}
      <div class="data-display">
        <h3>Post Details (ID: {postIdInput})</h3>
        <pre>{JSON.stringify(postById, null, 2)}</pre>
      </div>
    {/if}
    
    <h3>All Posts {#if !(isLoadingInitialData || isLoadingAction)}({allPosts.length}){/if}</h3>
    <div class="data-display">
      {#if allPosts.length > 0}
        <ul>
        {#each allPosts as post (post.id)}
          <li>
            <strong>ID: {post.id} - {post.title}</strong>
            <p>{post.content}</p>
            <small>User ID: {post.userId}</small>
          </li>
        {/each}
        </ul>
      {:else if !(isLoadingInitialData || isLoadingAction)}
        <p>No posts found or yet to load.</p>
      {/if}
    </div>
  </div>

  <!-- Categories Section -->
  <div class="section-container">
    <h2>Categories</h2>
    <div class="input-group">
      <label for="categoryId">Fetch Category by ID:</label>
      <input id="categoryId" type="number" bind:value={categoryIdInput} placeholder="Enter Category ID" />
      <button on:click={handleFetchCategoryById} disabled={isLoadingInitialData || isLoadingAction || categoryIdInput === ''}>Fetch Category</button>
    </div>
    {#if categoryById}
      <div class="data-display">
        <h3>Category Details (ID: {categoryIdInput})</h3>
        <pre>{JSON.stringify(categoryById, null, 2)}</pre>
      </div>
    {/if}

    <h3>All Categories {#if !(isLoadingInitialData || isLoadingAction)}({allCategories.length}){/if}</h3>
    <div class="data-display">
      {#if allCategories.length > 0}
        <ul>
        {#each allCategories as category (category.id)}
          <li>{category.name} (ID: {category.id})</li>
        {/each}
        </ul>
      {:else if !(isLoadingInitialData || isLoadingAction)}
        <p>No categories found or yet to load.</p>
      {/if}
    </div>
  </div>

  <!-- Comments Section -->
  <div class="section-container">
    <h2>Comments</h2>
    <div class="input-group">
      <label for="commentId">Fetch Comment by ID:</label>
      <input id="commentId" type="number" bind:value={commentIdInput} placeholder="Enter Comment ID" />
      <button on:click={handleFetchCommentById} disabled={isLoadingInitialData || isLoadingAction || commentIdInput === ''}>Fetch Comment</button>
    </div>
    {#if commentById}
      <div class="data-display">
        <h3>Comment Details (ID: {commentIdInput})</h3>
        <pre>{JSON.stringify(commentById, null, 2)}</pre>
      </div>
    {/if}

    <div class="input-group">
      <label for="parentCommentId">Fetch Replies for Comment ID:</label>
      <input id="parentCommentId" type="number" bind:value={parentCommentIdInput} placeholder="Enter Parent Comment ID" />
      <button on:click={handleFetchRepliesForComment} disabled={isLoadingInitialData || isLoadingAction || parentCommentIdInput === ''}>Fetch Replies</button>
    </div>
    {#if repliesForComment.length > 0}
      <div class="data-display">
        <h3>Replies for Comment ID: {parentCommentIdInput} ({repliesForComment.length})</h3>
        <ul>
        {#each repliesForComment as reply (reply.id)}
          <li>
            <p>{reply.content}</p>
            <small>Reply ID: {reply.id}, User ID: {reply.userId}, Created: {new Date(reply.createdAt).toLocaleString()}</small>
          </li>
        {/each}
        </ul>
      </div>
    {:else if parentCommentIdInput && !(isLoadingInitialData || isLoadingAction)}
        <p>No replies found for comment ID {parentCommentIdInput} or yet to load.</p>
    {/if}

    <h3>All Loaded Comments {#if !(isLoadingInitialData || isLoadingAction)}({allComments.length}){/if}</h3>
    <div class="data-display">
      {#if allComments.length > 0}
        <ul>
        {#each allComments as comment (comment.id)}
          <li>
            <p>{comment.content}</p>
            <small>ID: {comment.id}, Post ID: {comment.postId}, User ID: {comment.userId}, Parent: {comment.parentCommentId || 'N/A'}</small><br/>
            <small>Created: {new Date(comment.createdAt).toLocaleString()}</small>
          </li>
        {/each}
        </ul>
      {:else if !(isLoadingInitialData || isLoadingAction)}
        <p>No comments found or yet to load.</p>
      {/if}
    </div>
  </div>

</div>
