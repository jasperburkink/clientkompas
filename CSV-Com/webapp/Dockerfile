FROM node:21-alpine as builder

ARG ENVIRONMENT_NAME=ocdev
RUN echo Using Environment: ${ENVIRONMENT_NAME}

WORKDIR /app
COPY . .
RUN yarn install
RUN npm run build:${ENVIRONMENT_NAME}

FROM nginx:1.25.4
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=builder /app/build .
COPY --from=builder /app/nginx.conf /etc/nginx/nginx.conf
ENTRYPOINT ["nginx", "-g", "daemon off;"]