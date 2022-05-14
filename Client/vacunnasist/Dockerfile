#Primera Etapa
FROM node:14.15 as build-step
RUN mkdir -p /app
WORKDIR /app
COPY package.json /app
RUN npm install
COPY . /app
RUN npm run build --prod

FROM nginx:1.17.1-alpine
COPY --from=build-step /app/dist/vacunnasist /usr/share/nginx/html
EXPOSE 4200