import React, { Component } from 'react';
import { Button, ButtonToolbar } from 'react-bootstrap'

//components
import { Apartment } from './Apartment';
import { ChangeSearchData } from './ChangeSearchData'
import { Booking } from '../booking/Booking'
import { BookingParameters } from "../booking/BookingParameters"

//API
import * as ApartmentsAPI from '../../api/Apartment'

export class ApartmentPage extends Component {

    constructor(props) {
        super(props);
        this.dateStartRef = React.createRef();
        this.dateEndRef = React.createRef();
        var night = this.calculateNight(props.location.dateStart, props.location.dateEnd);
        this.state = {
            id: props.match.params.id,
            night:night,

            //booking
            bookingShow: false,
            showBookingForm: true,
            showError: false,
            showSuccess: false,
            booking:{
                adults: 1,
                children: 0,
                apartmentId: props.match.params.id,
                dateStart: props.location.dateStart,
                dateEnd: props.location.dateEnd,
                price: props.location.price,
            },

            //changeSearchData
            changeSearchDataShow: false,
            searchErrorShow: false,
            searchSuccessShow: false,
            checkAvailabilityButtonDisabled: true,
            showSelectDate:true,
            processingRequest:false,
            findApartment:{
                apartmentTypeId: props.match.params.id,
                dateStart: props.location.dateStart,
                dateEnd: props.location.dateEnd
            }
        }
    }

    calculateNight(dateStart, dateEnd){
        var night = null;

        if(dateStart && dateEnd){
            dateStart = new Date(dateStart);
            dateEnd = new Date(dateEnd);
            if(dateStart.getDate()<dateEnd.getDate())
                night = Math.ceil(Math.abs(dateEnd - dateStart) / (1000 * 60 * 60 * 24)); 
        }
        if(night != null){
            if(night == 1){
                night = "1 ночь";
            }
            else if(night > 1){
                night = `${night} ночей`;
            }
            else{
                night = null;
            }
        }

        return night;
    }

    setNight(){
        var night = this.calculateNight(this.state.booking.dateStart, this.state.booking.dateEnd);
        this.setState({night: night});
    }

    render() {

        const onChangeSearchDataForm = (e) => {
            if(e.target.id === "dateStart"){
                this.state.findApartment.dateStart = e.target.value;
                if (this.state.findApartment.dateEnd && new Date(this.state.findApartment.dateEnd) <= new Date(this.state.findApartment.dateStart)) {
                    this.state.findApartment.dateEnd = null;
                }
            }
            if(e.target.id === "dateEnd"){
                this.state.findApartment.dateEnd = e.target.value
            }
            if(this.state.findApartment.dateStart && this.state.findApartment.dateEnd && (this.state.findApartment.dateStart !== this.state.booking.dateStart || this.state.findApartment.dateEnd !== this.state.booking.dateEnd)){
                this.setState({checkAvailabilityButtonDisabled: false});
            }else{
                this.setState({checkAvailabilityButtonDisabled: true});
            }
        }

        const checkAvailability = async () => {
            this.setState({processingRequest: true});
            await ApartmentsAPI.getPriceIfApartmentTypeAvailable(this.state.findApartment).then((result) => {
                    this.setState({processingRequest: false, checkAvailabilityButtonDisabled: true, searchSuccessShow:true, showSelectDate: false, searchErrorShow: false, booking: {...this.state.booking, dateEnd: this.state.findApartment.dateEnd, dateStart: this.state.findApartment.dateStart , price: result}});
                    this.setNight();
            }).catch((error)=>{
                console.log(error);
                this.setState({processingRequest: false, searchErrorShow:true});
            })
        };

        return (
            <div>

                <Apartment id={this.state.id} dateStart={this.state.booking.dateStart} dateEnd={this.state.booking.dateEnd} price={this.state.booking.price} />

                <BookingParameters
                dateStart={this.state.booking.dateStart}
                dateEnd={this.state.booking.dateEnd}
                price={this.state.booking.price}
                adults={this.state.booking.adults}
                children={this.state.booking.children}
                changeParameters = {() => this.setState({changeSearchDataShow: true})}
                night ={this.state.night}
                onAdultsChange = {(e) => this.setState({booking: {...this.state.booking, adults: e}})}
                onChildrenChange = {(e) => this.setState({booking: {...this.state.booking, children: e}})}
                />

                <ButtonToolbar>
                    <Button
                    variant='primary' 
                    onClick={() => this.setState({bookingShow: true})}
                    disabled={!this.state.booking.dateStart || !this.state.booking.dateEnd}
                    >Забронировать</Button>
                </ButtonToolbar>




                <ChangeSearchData
                dateEndRef={this.dateEndRef}
                dateStartRef={this.dateStartRef}
                dateStart={this.state.booking.dateStart}
                dateEnd={this.state.booking.dateEnd}
                show={this.state.changeSearchDataShow} 
                onHide={() => this.setState({changeSearchDataShow:false, searchSuccessShow: false, showError: false, checkAvailabilityButtonDisabled: true, showSelectDate: true, searchErrorShow: false})} 
                checkAvailability={checkAvailability} 
                showError={this.state.searchErrorShow}
                showSuccess={this.state.searchSuccessShow}
                closeError={() => this.setState({searchErrorShow:false})} 
                onChangeSearchDataForm={onChangeSearchDataForm}
                checkAvailabilityButtonDisabled={this.state.checkAvailabilityButtonDisabled}
                showSelectDate={this.state.showSelectDate}
                processingRequest={this.state.processingRequest}
                />

                <Booking 
                show={this.state.bookingShow}
                onHide={() => this.setState({bookingShow:false, showBookingForm: true, showError: false, showSuccess: false})}
                showBookingForm={this.state.showBookingForm} 
                showError={this.state.showError} 
                showSuccess={this.state.showSuccess}
                booking={this.state.booking}
                showSuccessBooking={() => this.setState({showSuccess: true})}
                showErrorBooking={() => this.setState({showError: true})}
                closeBookingForm={() => this.setState({showBookingForm: false})}
                />

            </div> 
        );
    }
}