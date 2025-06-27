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
    import { isLoggedIn as isLoggedInService, getCurrentUser, logout } from "$lib/services/loginService.js";
    import { isLoggedIn, currentUser, updateAuthState } from "$lib/stores/authStore.js";
    import uploadSign from "$lib/images/upload-sign.svg";
    let showMenu = false;

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
        
        // Ajuster la position du menu si nécessaire
        if (showMenu) {
            setTimeout(() => {
                const dropdown = document.querySelector('.burger-dropdown');
                const burgerMenu = document.querySelector('.burger-menu');
                
                if (dropdown && burgerMenu) {
                    const dropdownRect = dropdown.getBoundingClientRect();
                    const viewportWidth = window.innerWidth;
                    const viewportHeight = window.innerHeight;
                    
                    // reset les transformations précédentes
                    dropdown.style.transform = '';
                    dropdown.style.right = '0';
                    dropdown.style.left = 'auto';
                    
                    // vérifier si le menu dépasse à droite
                    if (dropdownRect.right > viewportWidth - 10) {
                        const overflow = dropdownRect.right - viewportWidth + 20;
                        dropdown.style.transform = `translateX(-${overflow}px)`;
                    }
                    
                    // vérifier si le menu dépasse en bas
                    if (dropdownRect.bottom > viewportHeight - 10) {
                        dropdown.style.top = 'auto';
                        dropdown.style.bottom = 'calc(100% + 10px)';
                        
                        // ajuster la position de l'arrow pour le dropdown en bas
                        const arrow = dropdown.querySelector('::before');
                        if (arrow) {
                            dropdown.style.setProperty('--arrow-position', 'bottom');
                        }
                    }
                    
                    // pour les écrans  petits, s'assurer que la largeur minimale est utilisable
                    if (viewportWidth < 400) {
                        const maxWidth = Math.min(350, viewportWidth - 30);
                        dropdown.style.maxWidth = `${maxWidth}px`;
                        dropdown.style.width = 'max-content';
                    }
                }
            }, 50);
        }
    }

    async function checkAuthStatus() {
        const loggedInStatus = isLoggedInService();
        if (loggedInStatus) {
            try {
                const user = await getCurrentUser();
                if (user) {
                    updateAuthState(true, user);
                } else {
                    updateAuthState(false, null);
                }
            } catch (error) {
                console.error('Error getting current user:', error);
                updateAuthState(false, null);
            }
        } else {
            updateAuthState(false, null);
        }
    }

    async function handleLogout() {
        logout();
        showMenu = false;
        // Optionally redirect to home or login
        window.location.href = '/';
    }

    onMount(async () => {
        window.addEventListener('click', handleClickOutside);
        // Initialize auth state
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
               <a href="/new-post"> <img
                    src={uploadSign}
                    alt="🔔"
                />
                </a>
            </li>


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
                        {#if $isLoggedIn && $currentUser}
                            <li class="user-info">
                                <strong>{$currentUser.username}</strong>
                                <span>{$currentUser.email}</span>
                            </li>
                            <li><a href="/profile">Profile</a></li>
                            <li><a href="/about">About</a></li>
                            <li><a href="/ai">LibreScript AI</a></li>

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

        transition: 0.3s ease;
        object-fit: contain; 
        width: 40px;
        height: 40px;
        padding: 0.5rem;
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
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .burger-btn:hover {
        background: #e0e0e0;
    }
    .burger-dropdown {
        position: absolute;
        right: 0;
        top: calc(100% + 10px);
        width: max-content;
        min-width: 200px;
        max-width: min(350px, calc(100vw - 40px));
        background: #fff;
        border: 1px solid #e2e8f0;
        border-radius: 12px;
        box-shadow: 0 10px 40px rgba(0, 0, 0, 0.12);
        z-index: 1000;
        padding: 0.75rem 0;
        list-style: none;
        margin: 0;
        animation: dropdownFadeIn 0.2s ease-out;
        transform-origin: top right;
    }
    
    @keyframes dropdownFadeIn {
        from {
            opacity: 0;
            transform: scale(0.95) translateY(-10px);
        }
        to {
            opacity: 1;
            transform: scale(1) translateY(0);
        }
    }
    .burger-dropdown li {
        padding: 0.75rem 1.5rem;
        color: #374151;
        cursor: pointer;
        transition: all 0.2s ease;
        border-radius: 0;
        position: relative;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }
    
    .burger-dropdown li:hover {
        background: #f8fafc;
        color: #166fff;
    }
   
    .burger-dropdown a {
        color: inherit;
        text-decoration: none;
        display: block;
        width: 100%;
        font-weight: 500;
        font-size: 0.95rem;
    }
    
    .user-info {
        display: flex;
        flex-direction: column;
        padding: 1rem 1.5rem !important;
        background: #fff;
        border-bottom: 1px solid #e2e8f0;
        margin-bottom: 0.5rem;
        border-radius: 0;
        min-width: 0; /* Allow text to truncate */
    }
    
    .user-info strong {
        font-size: 1rem;
        color: #166fff;
        margin-bottom: 4px;
        font-weight: 600;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        max-width: 100%;
    }
    
    .user-info span {
        font-size: 0.85rem;
        color: #6b7280;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        max-width: 100%;
        opacity: 0.8;
    }
    
    .logout-btn {
        background: none;
        border: none;
        color: #dc2626;
        cursor: pointer;
        text-align: left;
        width: 100%;
        padding: 0;
        font-size: 0.95rem;
        font-family: inherit;
        font-weight: 500;
        transition: all 0.2s ease;
    }
    
    .logout-btn:hover {
        color: #b91c1c;
        background: #fef2f2;
    }
    

    /* Menu burger arrow indicator */
    .burger-dropdown::before {
        content: '';
        position: absolute;
        top: -6px;
        right: 20px;
        width: 12px;
        height: 12px;
        background: #fff;
        border: 1px solid #e2e8f0;
        border-bottom: none;
        border-right: none;
        transform: rotate(45deg);
        z-index: 1001;
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
            right: 0;
            left: auto;
            min-width: 250px;
            max-width: calc(100vw - 30px);
            width: max-content;
        }
        
        .burger-dropdown::before {
            right: 25px;
        }
        
        .user-info {
            padding: 0.875rem 1.25rem !important;
        }
        
        .user-info strong {
            font-size: 0.95rem;
        }
        
        .user-info span {
            font-size: 0.8rem;
        }
        
        .burger-dropdown li {
            padding: 0.625rem 1.25rem;
            font-size: 0.9rem;
        }
    }

     /* Pour les très petits écrans */
     @media (max-width: 360px) {
         .burger-dropdown {
             right: 0;
             min-width: 220px;
             max-width: calc(100vw - 20px);
             width: max-content;
         }
         
         .burger-dropdown::before {
             right: 20px;
         }
         
         .burger-dropdown li {
             padding: 0.5rem 1rem;
             font-size: 0.85rem;
         }
         
         .user-info {
             padding: 0.75rem 1rem !important;
         }
         
         .user-info strong {
             font-size: 0.9rem;
         }
         
         .user-info span {
             font-size: 0.75rem;
         }
     }

     /* Pour les écrans très étroits */
     @media (max-width: 320px) {
         .burger-dropdown {
             right: 0;
             min-width: 200px;
             max-width: calc(100vw - 15px);
             width: max-content;
         }
         
         .burger-dropdown::before {
             right: 15px;
         }
         
         .burger-dropdown li {
             padding: 0.5rem 0.875rem;
             font-size: 0.8rem;
         }
         
         .user-info {
             padding: 0.625rem 0.875rem !important;
         }
         
         .user-info strong {
             font-size: 0.85rem;
         }
         
         .user-info span {
             font-size: 0.7rem;
         }
     }
</style>
