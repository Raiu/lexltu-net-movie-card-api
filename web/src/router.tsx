import { createBrowserRouter, createRoutesFromElements, Route } from "react-router-dom";

import { MoviesPage } from "@pages/MoviesPage";
import App from "./App";


export const router = createBrowserRouter(
    createRoutesFromElements(
        <Route path="/" element={<App />}>
            <Route index element={<MoviesPage />} />
            <Route path="/movies" element={<MoviesPage />} />
        </Route>       
    )
);

