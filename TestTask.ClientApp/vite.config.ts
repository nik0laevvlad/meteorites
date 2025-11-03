import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3020,
    open: true,
    proxy: {
      '/api': {
        target: 'https://localhost:7050',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
