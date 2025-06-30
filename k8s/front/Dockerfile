FROM node:20-alpine AS builder
WORKDIR /frontend-web
COPY ../../frontend-web .
RUN npm install && npm run build

FROM nginx:alpine
COPY --from=builder /frontend-web/dist /usr/share/nginx/html
EXPOSE 80
