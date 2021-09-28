import React from 'react'
import * as ApartmentAPI from '../../../api/Apartment/Apartment';
import Movie from './MovieList-element'
import './MovieList.css'


export default function MovieList({ movies }) {
    const movieElements = movies.map(movie =>
        <li key={movie.id} className="movie-list__li col-lg-3"><Movie movie={movie} /></li>
    )
    return (
        <div className="list_container">
            <ul className="movies_table row">
                {movieElements}
            </ul>
        </div>
    )
}