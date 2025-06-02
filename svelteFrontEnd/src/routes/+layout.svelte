<script>
  // You can add imports for global styles or other layout logic here if needed
  // import '../app.css'; // Example: if you have global styles in src/app.css
</script>

<div class="layout-container">
  <header>
    <!-- You can put a global header/navbar here -->
  </header>

  <main>
    <slot /> <!-- This is crucial for page content to render -->
  </main>

  <footer>
    <!-- You can put a global footer here -->
  </footer>
</div>

<style>
  /* Optional: Add some basic styling for the layout itself */
  .layout-container {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
  }

  main {
    flex-grow: 1; /* Ensures main content takes up available space */
    /* Add padding or other styles as needed */
  }

  /* Example: 
  header, footer {
    padding: 1rem;
    background-color: #f0f0f0;
    text-align: center;
  }
  */
</style>
