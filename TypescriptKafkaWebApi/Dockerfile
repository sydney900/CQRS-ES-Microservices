FROM node:9-alpine as builder
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /usr/src/app
COPY ["./TypescriptKafkaWebApi/package.json", "./TypescriptKafkaWebApi/package-lock.json", "./"]
RUN npm install --silent
COPY ["./TypescriptKafkaWebApi/tsconfig.json", "./TypescriptKafkaWebApi/tslint.json", "./"]
COPY ./TypescriptKafkaWebApi/src/ ./src/
COPY ./TypescriptKafkaWebApi/.env.docker ./.env
RUN npm run build

FROM node:9-alpine as builder
ENV NODE_ENV production
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /usr/src/app
COPY --from=builder /usr/src/app/dist /usr/src/app/
EXPOSE 80
CMD node server.js
