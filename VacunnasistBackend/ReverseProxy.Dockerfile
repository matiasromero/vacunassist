FROM nginx:1.19.9

COPY ./nginx.dev.conf /etc/nginx/conf.d/default.conf