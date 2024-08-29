import { MovieApiClient, MovieResDto } from '@/client/moviedb';
import { useEffect, useState } from 'react';

function MoviesPage() {
    const [movies, setMovies] = useState<MovieResDto[]>();
    const api = new MovieApiClient("http://127.0.0.1:5244");
    
    useEffect(() => {
        const controller = new AbortController();
        api.moviesAll().then((data) => 
            data && setMovies(data)
        ).catch(console.error);

        return () => {
            controller.abort();
        };
    })
    return (
        <div>
            MoviesPage
            {movies && movies.map((movie, index) => <div key={index}>{movie.title}</div>)}
        </div>
    )
}

export default MoviesPage;