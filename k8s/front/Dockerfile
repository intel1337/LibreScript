FROM node:20-alpine AS builder
WORKDIR /frontend-web
COPY ../../frontend-web/package*.json ./
RUN npm ci
COPY ../../frontend-web .
RUN npm run build

FROM nginx:alpine
# Copy built files from builder stage (SvelteKit outputs to 'build' directory)
COPY --from=builder /frontend-web/build /usr/share/nginx/html

# Add nginx configuration for SPA routing
RUN echo 'server { \
    listen 80; \
    server_name localhost; \
    root /usr/share/nginx/html; \
    index index.html; \
    \
    location / { \
        try_files $uri $uri/ /index.html; \
    } \
    \
    # Handle static assets \
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ { \
        expires 1y; \
        add_header Cache-Control "public, immutable"; \
    } \
}' > /etc/nginx/conf.d/default.conf

EXPOSE 80
