import React, { Component } from 'react';
import { Modal, Button, Row, Col, Form, Spinner, Alert} from 'react-bootstrap';
import { Formik } from "formik";

//API
import * as BookingAPI from '../../api/Booking';

export class Booking extends Component {
  constructor(props) {
    super(props);
    this.state = {
      showLoading: false,
      showBookingNumber: false,
      bookingNumber: String,
      booking: this.props.booking
    }
  }
  
  render() {

    const sendBooking = async (booking) =>{
        this.props.closeBookingForm();
        this.setState({showLoading: true});
        this.setState({booking:{ ...this.props.booking, clientName: booking.name, phoneNumber: booking.phone, email: booking.email, description: booking.description}});
        await BookingAPI.createBooking(this.state.booking).then((result) => {
                if(result.bookingNumber !== null && result.bookingNumber !== undefined){
                    this.setState({bookingNumber: result.bookingNumber, showBookingNumber: true, showLoading: false});
                }else{
                    this.setState({bookingNumber: null, showBookingNumber: false, showLoading: false});
                }
                this.props.showSuccessBooking();
        }).catch((error)=>{
            console.log(error);
            this.setState({showLoading: false});
            this.props.showErrorBooking();
        })
    };

    const validate = (values) =>{
        const errors = {};
        if (!values.name || values.name.trim() == "") {
          errors.name = 'Обязательное поле';
        } else if (values.name.trim().length<3 || values.name.trim().length>100) {
          errors.name = 'Имя должно быть не меньше 3 и не больше 100 символов';
        }
        if (!values.email || values.name.trim() == "") {
          errors.email = 'Обязательное поле';
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)) {
          errors.email = 'Неверный адрес электронной почты';
        }
        if (values.description && values.description.length>1000) {
          errors.description = 'Комментарий не должен привышать 1000 символов';
        }
        if (!values.phone || values.phone.trim() == "") {
          errors.phone = 'Обязательное поле';
        } else if (!/^\+?3?8?(0[5-9][0-9]\d{7})$/i.test(values.phone)) {
          errors.phone = 'Неверный формат номера телефона';
        }

        return errors;
    };
    
    function BookingForm() {
      return (
        <Formik
          validate = {validate}
          onSubmit={sendBooking}
          initialValues={{
            name: '',
            phone: '+380',
            email: '',
            description: ''
          }}
        >
          {({
            handleSubmit,
            handleChange,
            values,
            touched,
            errors,
          }) => (
            <Form noValidate onSubmit={handleSubmit}>
        
                <Form.Group as={Row} className="mb-3" >
                    <Form.Label column sm={2}>Имя</Form.Label>     
                    <Col sm={10}>
                        <Form.Control 
                        type="text"
                        name="name"
                        value={values.name}
                        onChange={handleChange}
                        isValid={touched.name && !errors.name}
                        isInvalid={!!errors.name}
                        />
                        <Form.Control.Feedback type="invalid">{errors.name}</Form.Control.Feedback>
                    </Col>
                </Form.Group>
            
                <Form.Group as={Row} className="mb-3" >
                    <Form.Label column sm={2}>Телефон</Form.Label>     
                    <Col sm={10}>
                        <Form.Control 
                        type="text"
                        name="phone"
                        value={values.phone}
                        onChange={handleChange}
                        isValid={touched.phone && !errors.phone}
                        isInvalid={!!errors.phone}
                        />
                        <Form.Control.Feedback type="invalid">{errors.phone}</Form.Control.Feedback>
                    </Col>
                </Form.Group>
            
                <Form.Group as={Row} className="mb-3" >
                    <Form.Label column sm={2}>Email</Form.Label>
                    <Col sm={10}>
                        <Form.Control 
                        type="email"
                        name="email"
                        value={values.email}
                        onChange={handleChange}
                        isValid={touched.email && !errors.email}
                        isInvalid={!!errors.email}
                        />
                        <Form.Control.Feedback type="invalid">{errors.email}</Form.Control.Feedback>
                    </Col>
                </Form.Group>
                
                <Form.Group as={Row} className="mb-3" >
                    <Form.Label column sm={2}>Комментарий</Form.Label>
                        <Col sm={10}>
                            <Form.Control 
                            as="textarea"
                            name="description"
                            value={values.description}
                            onChange={handleChange}
                            isValid={touched.description && !errors.description}
                            isInvalid={!!errors.description}
                            />
                            <Form.Control.Feedback type="invalid">{errors.description}</Form.Control.Feedback>
                        </Col>
                </Form.Group>
            
                <Button type="submit" >Отправить</Button>
            
            </Form>
          )}
        </Formik>
      );
    }
    return (
        <Modal {...this.props} size="lg" aria-labelledby="contained-modal-title-vcenter" centered >

          <Modal.Header closeButton>
            <Modal.Title id="contained-modal-title-vcenter">
                Введите свои данные
            </Modal.Title>
          </Modal.Header>

          <Modal.Body>

            { this.props.showBookingForm ? <BookingForm /> : null }   
        
            { this.state.showLoading ?
                <Alert variant="light" class="text-center">
                    <Alert.Heading>Ваши данные обрабатываются. Пожалуйста, подождите.</Alert.Heading>
                    <br/>
                    <Spinner
                    animation="border"  
                    style={{height: 100, width: 100}} 
                    role="status"
                    >
                      <span className="visually-hidden">Загрузка...</span>
                    </Spinner>
                </Alert>
            : null }       

            <Alert 
            show={this.props.showError} 
            variant="danger"
            onClose={this.props.onHide} 
            dismissible
            >
              <Alert.Heading>Ошибка!</Alert.Heading>
                <p>Произошла ошибка. Попробуйте ещё раз.</p>
            </Alert> 

            <Alert
            variant="success"
            show={this.props.showSuccess}
            >
              <Alert.Heading>Поздравляю!</Alert.Heading>
                <p>Ваше бронирование успешно создано. Ожидайте сообщения на почте. В скором времени мы с Вами свяжемся, чтобы подтвердить Ваш заказ.</p>
                { this.state.showBookingNumber ? <p> Номер бронирования: {this.state.bookingNumber}</p>  : null } 
                <hr />
                <div className="d-flex justify-content-end">
                  <Button variant="outline-success" onClick={() => window.location.replace("/")}>На главную</Button>
                </div>
            </Alert>
            
          </Modal.Body>

      </Modal>    
    );
  }
}