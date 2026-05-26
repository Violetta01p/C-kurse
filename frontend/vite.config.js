import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
// base вказує назву репозиторію на GitHub — потрібно для правильних шляхів на GitHub Pages
export default defineConfig({
    plugins: [react()],
    base: '/C-kurse/',
});
