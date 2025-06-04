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
    import { isLoggedIn } from "$lib/services/loginService.js";
    let showMenu = false;
    let loggedIn = false;

    // Ferme le menu si on clique ailleurs
    function handleClickOutside(event) {
        if (!event.target.closest('.burger-menu')) {
            showMenu = false;
        }
    }

    function toggleMenu() {
        loggedIn = isLoggedIn(); // Vérifie à chaque ouverture
        showMenu = !showMenu;
    }

    onMount(() => {
        window.addEventListener('click', handleClickOutside);
        loggedIn = isLoggedIn();
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
                        {#if loggedIn}
                            <li><a href="/dashboard">Dashboard</a></li>
                            <li><a href="/settings">Settings</a></li>
                            <li><a href="/earnings">Earnings</a></li>
                            <li class="divider"></li>
                            <li><a href="/logout">Sign out</a></li>
                        {:else}
                            <li><a href="/login">Login</a></li>
                            <li><a href="/register">Register</a></li>
                        {/if}
                    </ul>
                {/if}
            </li>
            <!-- <li id="profile">
                <a href="./profile"
                    ><img
                        src="https://img.freepik.com/premium-vector/man-avatar-profile-picture-vector-illustration_268834-538.jpg?semt=ais_hybrid&w=740"
                        alt=""
                    /></a
                >
            </li> -->
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
    }

    nav ul {
        list-style: none;
        display: flex;
        gap: 1rem;
        align-items: center;
        padding-right: 0.5rem; /* Ajouté pour éviter que le dernier item soit collé au bord */
    }

    nav ul li {
        display: inline;
    }

    nav ul li img {
        width: 24px; /* Adjust size as needed */
        height: 24px; /* Adjust size as needed */
    }

    nav ul li a {
        text-decoration: none;
        color: #333;
    }
    nav ul li a:hover {
        text-decoration: underline;
    }
    nav ul li img {
        width: 24px; /* Adjust size as needed */
        height: 24px; /* Adjust size as needed */
    }

    nav ul li a {
        text-decoration: none;
        color: #333;
    }
    #rightside {
        margin-left: auto; /* Pushes the right side items to the right */
    }
    #logo img {
        width: 24px; /* Adjust size as needed */
        height: 24px; /* Adjust size as needed */
    }
    #notifications {
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        border-radius: 50%; /* Makes the notification icon circular */
        padding: 0.5rem; /* Adds some padding around the icon */
        transition: 0.3s ease;
    }
    #notifications:hover {
        background-color: #e0e0e0; /* Changes background on hover */
        cursor: pointer; /* Changes cursor to pointer on hover */
    }
    #notifications img {
        width: 24px; /* Adjust size as needed */
        height: 24px; /* Adjust size as needed */
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
    .burger-dropdown li:hover {
        background: #f0f0f0;
    }
    .burger-dropdown .divider {
        border-bottom: 1px solid #eee;
        margin: 0.3rem 0;
        height: 0;
        padding: 0;
    }

    .burger-menu {
        /* Suppression de la marge droite qui décale le menu */
        margin-right: 0;
    }
    @media (max-width: 600px) {
        .burger-menu {
            margin-right: 0;
        }
        .burger-dropdown {
            right: 0;
            left: auto;
            min-width: 140px;
            max-width: 90vw;
        }
        .burger-btn img {
            width: 36px;
            height: 36px;
        }
    }
</style>
