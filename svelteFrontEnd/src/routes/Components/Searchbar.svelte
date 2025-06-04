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
        width: 15%;
        height: 2rem; /* Set a fixed height */

    }

    .searchbar-container img {
        margin-right: 0.5rem;
        height: 1.5rem;
        width: 1.5rem;
    }

    .search-input {
        flex-grow: 1;
        padding: 0.5rem 0.5rem;
        border: none;
        border-radius: 4px;
        font-size: 1rem;
        height: 100%; /* Match container height */
        background-color: #ffffff; /* Match container bg */
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
</style>