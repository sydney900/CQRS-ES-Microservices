FROM node:9-alpine as builder
ENV NODE_ENV production
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /usr/src/app
COPY ["package.json", "package-lock.json*", "./"]
RUN npm install --production --silent
COPY ["tsconfig.json", "tslint.json", "./"]
COPY ./src ./src/
COPY ./.env.docker ./.env
RUN npm run build

FROM node:9-alpine as builder
ENV NODE_ENV production
RUN apk update && apk upgrade && apk add --no-cache bash git openssh
WORKDIR /usr/src/app
COPY --from=builder /usr/src/app/dist /usr/src/app/
EXPOSE 80
CMD node server.js
