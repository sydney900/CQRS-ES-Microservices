FROM node:9-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /app
COPY ["package.json", "package-lock.json*", "./"]
RUN npm install --production --silent
COPY ["tsconfig.json", "tslint.json", "angular.json", "default.conf", "./"]
COPY ./src/ ./src/
RUN npm run build

FROM nginx:1.13.9-alpine
COPY --from=builder /app/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/website /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]