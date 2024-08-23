import { WebSocket } from './websocket.mjs';
import express from 'express';
import cors from 'cors';
import http from 'http';

const app = express();
app.use(cors());

const server = http.createServer(app);
new WebSocket(server);

server.listen(3000, () => {
  console.log('Server is running on ws://localhost:3000');
});
