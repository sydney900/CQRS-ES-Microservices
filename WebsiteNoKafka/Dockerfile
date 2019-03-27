FROM node:11-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /app
COPY ["WebsiteNoKafka/package.json", "WebsiteNoKafka/package-lock.json", "./"]
RUN npm install
COPY ["WebsiteNoKafka/tsconfig.json", "WebsiteNoKafka/tslint.json", "WebsiteNoKafka/angular.json", "WebsiteNoKafka/default.conf", "WebsiteNoKafka/sydney900.pem", "WebsiteNoKafka/sydney900.rsa", "./"]
COPY WebsiteNoKafka/src/ ./src/
RUN npm run build

FROM nginx:1.14.1-alpine
COPY --from=builder ["/app/default.conf", "/app/sydney900.pem", "/app/sydney900.rsa", "/etc/nginx/conf.d/"]
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/website /usr/share/nginx/html
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]
