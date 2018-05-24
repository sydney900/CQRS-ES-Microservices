FROM node:9-alpine
ENV NODE_ENV production
WORKDIR /usr/src/app
COPY ["package.json", "package-lock.json*", "./"]
RUN npm install --production --silent && mv node_modules ../
# COPY ["webpack.config.js", "index.api.js", "./"]
COPY ["index.api.js", "./"]
COPY ./server server/
EXPOSE 80
EXPOSE 27017
CMD node index.api.js