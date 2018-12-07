FROM node:10-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /app
COPY ["package.json", "package-lock.json", "./"]
RUN npm install
COPY ["tsconfig.json", "tslint.json", "angular.json", "default.conf", "sydney900.pem", "sydney900.rsa", "./"]
COPY src/ ./src/
RUN npm run build

FROM nginx:1.14.1-alpine
COPY --from=builder ["/app/default.conf", "/app/sydney900.pem", "/app/sydney900.rsa", "/etc/nginx/conf.d/"]
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/website /usr/share/nginx/html
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]
