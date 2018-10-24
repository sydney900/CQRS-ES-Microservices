FROM node:10-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /app
COPY ["package.json", "package-lock.json", "./"]
RUN npm install
COPY ["tsconfig.json", "tslint.json", "angular.json", "default.conf", "./"]
COPY src/ ./src/
RUN npm run build

FROM nginx:1.15-alpine
COPY --from=builder /app/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/website /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
