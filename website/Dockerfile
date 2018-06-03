FROM node:9-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /app
COPY ["website/package.json", "website/package-lock.json", "./"]
RUN npm install
COPY ["website/tsconfig.json", "website/tslint.json", "website/angular.json", "website/default.conf", "./"]
COPY website/src/ ./src/
RUN npm run build

FROM nginx:1.13.9-alpine
COPY --from=builder /app/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/website /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]