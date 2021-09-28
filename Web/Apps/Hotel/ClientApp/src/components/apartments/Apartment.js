import React, { Component } from 'react';
//import Carousel from 'react-bootstrap/Carousel'
import ImageGallery from 'react-image-gallery';
import "react-responsive-carousel/lib/styles/carousel.min.css";
//import { Carousel } from 'react-responsive-carousel';
import Carousel from 'react-gallery-carousel';
import 'react-gallery-carousel/dist/index.css';

import './Apartment.css'

//API
import * as ApartmentAPI from '../../api/Apartment';


import GoogleMapReact from 'google-map-react';
import { Map, GoogleApiWrapper, Marker } from 'google-maps-react';
const AnyReactComponent = ({ text }) => <div>{text}</div>;
  

export class Apartment extends Component {

    constructor(props) {
        super(props);
        this.state = {
            id: props.id,
            dateStart: props.dateStart,
            dateEnd: props.dateEnd,
            price: props.price,
            apartment: {
                id: String,
                title: String,
                description: String,
                mainImage: String,
                images: []
            },
            images:[]
        }
    }

    componentDidMount = async () => {
        await ApartmentAPI.getApartment(this.state.id)
            .then((result) => {
                this.setState({ apartment: result });
            });
    };

    render() {
        const images = [];

        if (this.state.apartment.images.length > 0) {
            let item = { values: [] }
            for (let i = this.state.apartment.images.length - 1; i >= 0; i--) {
                item.values.push(
                    <Carousel.Item>
                        <img
                        className="d-block w-100"
                        src={`data:image/png;base64,${this.state.apartment.images[i]}`}
                        />
                    </Carousel.Item>
                );
            }

            images.push(item);
        }

        const images1 = [
            {
              original: 'https://picsum.photos/id/1018/1000/600/',
              thumbnail: 'https://picsum.photos/id/1018/250/150/',
            },
            {
              original: 'https://picsum.photos/id/1015/1000/600/',
              thumbnail: 'https://picsum.photos/id/1015/250/150/',
            },
            {
              original: 'https://picsum.photos/id/1019/1000/600/',
              thumbnail: 'https://picsum.photos/id/1019/250/150/',
            },
          ];

          const images2 = this.state.apartment.images.map((image) => ({
            src:`data:image/jpg;base64,${image}`,
            sizes: '(max-width: 100px) 40px, (max-width: 200px) 70px, 100px',
          }));

        return (
            <div>
                <div style={{width:600, height:400}}>
                    <Carousel images={images2} />
                </div>

                <Map
          google={this.props.google}
          zoom={8}
          style={{width: '100%', height: '100%'}}
          initialCenter={{ lat: 47.444, lng: -122.176}}
        >
          <Marker position={{ lat: 48.00, lng: -122.00}} />
        </Map>

                <div style={{ height: '100vh', width: '100%' }}>
        <GoogleMapReact
          //bootstrapURLKeys={{ key: "AIzaSyDiKc4HxX5G7EfneIZBN_Hlk2_luoT_yvo"}}
          defaultCenter={{lat: 46.1145651, lng: 32.9105010}}
          defaultZoom={15}
          //yesIWantToUseGoogleMapApiInternals={true}
        >
            
            <Marker position={{ lat: 46.00, lng: 32.00}} />
        </GoogleMapReact>
      </div>
                <img src={`data:image/png;base64,${this.state.apartment.mainImage}`} />

                <h1>{this.state.apartment.title}</h1>
                <div style={{fontSize:40}} dangerouslySetInnerHTML={{ __html: this.state.apartment.description }} />
            </div>
        );
    }
}

export default GoogleApiWrapper({
    //apiKey: 'TOKEN HERE'
  })(Apartment);