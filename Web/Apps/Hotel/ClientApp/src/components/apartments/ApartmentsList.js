import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import Spinner from 'react-bootstrap/Spinner'

//API
import * as ApartmentAPI from '../../api/Apartment';

export class ApartmentsList extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading:true,
            dateStart: props.dateStart,
            dateEnd: props.dateEnd,
            apartments: {
                id: String,
                title: String,
                mainImage: String,
                price: Number
            }
        }
    }

    componentDidMount = async () => {
        await ApartmentAPI.getApartments({
            dateStart: this.state.dateStart,
            dateEnd: this.state.dateEnd
        })
            .then((result) => {
                this.setState({ apartments: result.apartmentTypes, loading:false });
            });
    };

    render() {
        const apartments = [];

        if (this.state.apartments.length > 0) {
            let item = { values: [] }
            for (let i = this.state.apartments.length - 1; i >= 0; i--) {
                var price;
                if (this.state.apartments[i].price !== null && this.state.apartments[i].price > 0) {
                    price = `Цена: ${this.state.apartments[i].price}`;
                } else {
                    price = "Цену уточняйте";
                }

                item.values.push(
                    <div className="apartments_list__item">

                        <img className="apartments_list__item__main_img" src={`data:image/jpeg;base64,${this.state.apartments[i].mainImage}`}/>

                        <div className="apartments_list__item__text">
                            <Link to={{ pathname: "/apartment/" + this.state.apartments[i].id, price: this.state.apartments[i].price, dateStart: this.state.dateStart, dateEnd: this.state.dateEnd }} >
                                <div className="apartments_list__item__text__title">
                                    <h3>{this.state.apartments[i].title}</h3>
                                </div>
                            </Link>
                            <div className="apartments_list__item__text__price"> 
                                    <h3>{price}</h3>
                            </div> 
                        </div>

                    </div>
                );
            }

        apartments.push(item);
        }

        return (
            <div className="apartments_list">

                {this.state.loading ?                 
                    <Spinner animation="border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </Spinner> 
                : null}

                {
                    apartments.map(el => {
                    return el.values
                    })
                }
            </div> 
        );
    }
}