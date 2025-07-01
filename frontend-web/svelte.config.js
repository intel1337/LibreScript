import adapter from '@sveltejs/adapter-static';

/** @type {import('@sveltejs/kit').Config} */
const config = {
	kit: {
		// Use static adapter for Docker deployment
		adapter: adapter({
			// Configure for static hosting
			fallback: 'index.html'
		})
	}
};

export default config;
