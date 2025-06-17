<div class="searchbar-container">
    <img src="https://img.icons8.com/ios_filled/512/search.png" alt="🔍">
    <input type="text" placeholder="Search..." class="search-input" />
</div>

<style>
    .searchbar-container {
        display: flex;
        align-items: center;
        padding: 0.5rem;
        background-color: #ffffff; 
        border-radius: 6px;
        width: 100%;
        max-width: 400px;
        min-width: 200px;
        height: 2rem;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    }

    .searchbar-container img {
        margin-right: 0.5rem;
        height: 1.5rem;
        width: 1.5rem;
        flex-shrink: 0;
    }

    .search-input {
        flex-grow: 1;
        padding: 0.5rem;
        border: none;
        border-radius: 4px;
        font-size: 1rem;
        height: 100%;
        background-color: #ffffff;
        width: 100%;
        min-width: 0;
    }

    .search-input:focus {
        outline: none;
        border-style: none;
        box-shadow: 0 0 2px #007bff;
    }
    
    .search-input:active{
        border-style: none;
        outline: none;
        box-shadow: 0 0 2px #007bff;
    }

    /* Mobile responsive adjustments */
    @media (max-width: 768px) {
        .searchbar-container {
            min-width: 150px;
            max-width: 300px;
            padding: 0.4rem;
        }
        
        .searchbar-container img {
            height: 1.2rem;
            width: 1.2rem;
            margin-right: 0.4rem;
        }
        
        .search-input {
            font-size: 0.9rem;
            padding: 0.4rem;
        }
    }

    @media (max-width: 480px) {
        .searchbar-container {
            min-width: 120px;
            max-width: 250px;
            padding: 0.3rem;
        }
        
        .searchbar-container img {
            height: 1rem;
            width: 1rem;
            margin-right: 0.3rem;
        }
        
        .search-input {
            font-size: 0.85rem;
            padding: 0.3rem;
        }
        
        .search-input::placeholder {
            font-size: 0.8rem;
        }
    }
</style>