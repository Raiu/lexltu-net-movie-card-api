import { defineConfig } from "vite";
import path from "path";
import react from "@vitejs/plugin-react-swc";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@components": path.resolve(__dirname, "./src/components"),
      "@context": path.resolve(__dirname, "./src/context"),
      "@data": path.resolve(__dirname, "./src/data"),
      "@helpers": path.resolve(__dirname, "./src/helpers"),
      "@interfaces": path.resolve(__dirname, "./src/interfaces"),
      "@models": path.resolve(__dirname, "./src/models"),
      "@pages": path.resolve(__dirname, "./src/pages"),
      "@": path.resolve(__dirname, "./src"),
    },
  },
});
