<script>
    import { onMount } from "svelte";
    import SearchBar from "./Searchbar.svelte";
    import {
        Button,
        Dropdown,
        DropdownItem,
        ToolbarButton,
        DropdownDivider,
    } from "flowbite-svelte";
    import { DotsHorizontalOutline, DotsVerticalOutline } from "flowbite-svelte-icons";
    import { isLoggedIn, getCurrentUser, logout } from "$lib/services/loginService.js";
    
    let showMenu = false;
    let loggedIn = false;
    let currentUser = null;

    // Ferme le menu si on clique ailleurs
    function handleClickOutside(event) {
        if (!event.target.closest('.burger-menu')) {
            showMenu = false;
        }
    }

    async function toggleMenu() {
        // Check authentication status when opening menu
        await checkAuthStatus();
        showMenu = !showMenu;
    }

    async function checkAuthStatus() {
        loggedIn = isLoggedIn();
        if (loggedIn) {
            try {
                currentUser = await getCurrentUser();
                if (!currentUser) {
                    loggedIn = false;
                }
            } catch (error) {
                console.error('Error getting current user:', error);
                loggedIn = false;
                currentUser = null;
            }
        } else {
            currentUser = null;
        }
    }

    async function handleLogout() {
        logout();
        loggedIn = false;
        currentUser = null;
        showMenu = false;
        // Optionally redirect to home or login
        window.location.href = '/';
    }

    onMount(async () => {
        window.addEventListener('click', handleClickOutside);
        await checkAuthStatus();
        return () => window.removeEventListener('click', handleClickOutside);
    });
</script>

<header>
    <nav>
        <ul>
            <li id="logo">
                <img
                    src="https://www.svgrepo.com/show/476667/develop.svg"
                    alt="❤️"
                />
            </li>
            <li id="logotext"><a href="/"><strong>LibreScript</strong></a></li>
            <li id="rightside"><SearchBar /></li>
            <li id="notifications">
                <img
                    src="https://www.svgrepo.com/show/488303/notification-bell.svg"
                    alt="🔔"
                />
            </li>

            <!-- Menu burger custom sans Flowbite -->
            <li class="burger-menu" style="position: relative;">
                <button class="burger-btn" on:click={toggleMenu} aria-label="Ouvrir le menu">
                    <img
                        src="https://img.freepik.com/premium-vector/man-avatar-profile-picture-vector-illustration_268834-538.jpg?semt=ais_hybrid&w=740"
                        alt="Profil"
                        style="width: 40px; height: 40px; border-radius: 50%; object-fit: cover;"
                    />
                </button>
                {#if showMenu}
                    <ul class="burger-dropdown">
                        {#if loggedIn && currentUser}
                            <li class="user-info">
                                <strong>{currentUser.username}</strong>
                                <span>{currentUser.email}</span>
                            </li>
                            <li class="divider"></li>
                            <li><a href="/dashboard">Dashboard</a></li>
                            <li><a href="/settings">Settings</a></li>
                            <li><a href="/earnings">Earnings</a></li>
                            <li class="divider"></li>
                            <li><button on:click={handleLogout} class="logout-btn">Sign out</button></li>
                        {:else}
                            <li><a href="/login">Login</a></li>
                            <li><a href="/register">Register</a></li>
                        {/if}
                    </ul>
                {/if}
            </li>
        </ul>
    </nav>
</header>

<style>
    @import url("https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap");

    header {
        font-family: "Roboto";
        background-color: #f0f0f0;
        border-bottom: 1px solid #ccc;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        width: 100%;
        position: sticky;
        top: 0;
        z-index: 100;
    }

    nav ul {
        list-style: none;
        display: flex;
        gap: 1rem;
        align-items: center;
        padding: 0.5rem 1rem;
        margin: 0;
        justify-content: space-between;
        flex-wrap: wrap;
    }

    nav ul li {
        display: inline;
    }

    nav ul li img {
        width: 24px;
        height: 24px;
    }

    nav ul li a {
        text-decoration: none;
        color: #333;
    }
    nav ul li a:hover {
        text-decoration: underline;
    }

    #rightside {
        flex: 1;
        display: flex;
        justify-content: center;
        margin: 0 1rem;
    }
    
    #logo img {
        width: 24px;
        height: 24px;
    }
    
    #notifications {
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        border-radius: 50%;
        padding: 0.5rem;
        transition: 0.3s ease;
    }
    
    #notifications:hover {
        background-color: #e0e0e0;
        cursor: pointer;
    }
    
    #notifications img {
        width: 24px;
        height: 24px;
    }

    #logotext {
        font-size: 1.5rem;
        font-weight: bold;
        color: #333;
    }

    /* Styles pour le menu burger */
    .burger-btn {
        background: none;
        border: none;
        cursor: pointer;
        padding: 0.5rem;
        border-radius: 50%;
        transition: background 0.2s;
    }
    .burger-btn:hover {
        background: #e0e0e0;
    }
    .burger-dropdown {
        position: absolute;
        right: 0;
        top: 120%;
        min-width: 140px;
        background: #fff;
        border: 1px solid #ccc;
        border-radius: 8px;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
        z-index: 1000;
        padding: 0.5rem 0;
        list-style: none;
        margin: 0;
    }
    .burger-dropdown li {
        padding: 0.5rem 1.2rem;
        color: #333;
        cursor: pointer;
        transition: background 0.15s;
    }
   
    .burger-dropdown a {
        color: #333;
        text-decoration: none;
        display: block;
        width: 100%;
    }
    
    .user-info {
        display: flex;
        flex-direction: column;
        padding: 0.75rem 1.2rem !important;
        background: #f8f9fa;
        border-bottom: 1px solid #e0e0e0;
        margin-bottom: 0.25rem;
    }
    
    .user-info strong {
        font-size: 0.95rem;
        color: #0969da;
        margin-bottom: 2px;
    }
    
    .user-info span {
        font-size: 0.8rem;
        color: #666;
    }
    
    .logout-btn {
        background: none;
        border: none;
        color: #dc3545;
        cursor: pointer;
        text-align: left;
        width: 100%;
        padding: 0;
        font-size: inherit;
        font-family: inherit;
        transition: color 0.15s;
    }
    
    .logout-btn:hover {
        color: #c82333;
        background: #f8f9fa;
    }
    
    .divider {
        height: 1px;
        background: #e0e0e0;
        margin: 0.5rem 0;
        padding: 0 !important;
    }

    /* Mobile responsive adjustments */
    @media (max-width: 768px) {
        nav ul {
            padding: 0.5rem;
            gap: 0.5rem;
        }
        
        #logotext {
            font-size: 1.2rem;
        }
        
        #rightside {
            margin: 0 0.5rem;
        }
        
        nav ul li img {
            width: 20px;
            height: 20px;
        }
        
        #notifications img {
            width: 20px;
            height: 20px;
        }
    }

    @media (max-width: 480px) {
        nav ul {
            flex-wrap: wrap;
            justify-content: center;
            gap: 0.25rem;
        }
        
        #logo, #logotext {
            order: 1;
        }
        
        #rightside {
            order: 2;
            width: 100%;
            margin: 0.5rem 0;
        }
        
        #notifications, .burger-menu {
            order: 3;
        }
        
        #logotext {
            font-size: 1rem;
        }
        
        .burger-dropdown {
            right: -50px;
            min-width: 120px;
        }
    }
</style>
