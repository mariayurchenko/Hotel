import React, { Component } from 'react';
import { Modal, Button, Alert, Spinner} from 'react-bootstrap';

//components
import { SelectDate } from '../SelectDate';

export class ChangeSearchData extends Component {
    constructor(props) {
      super(props);
  }

  render() {

    return (
        <Modal {...this.props} size="lg" aria-labelledby="contained-modal-title-vcenter" centered >

          <Modal.Header closeButton>
            <Modal.Title id="contained-modal-title-vcenter">
                Измените данные поиска
            </Modal.Title>
          </Modal.Header>

          <Modal.Body>

            { this.props.showSelectDate ?
            <SelectDate
            dateStart={this.props.dateStart}
            dateEnd={this.props.dateEnd}
            dateEndRef={this.props.dateEndRef}
            dateStartRef={this.props.dateStartRef}
            onChangeSearchDataForm={this.props.onChangeSearchDataForm}
            /> : null }

            <Alert variant="success" show={this.props.showSuccess}>
              <Alert.Heading>Места найдены!</Alert.Heading>
                <p>
                  Параметры были успешно применены.
                </p>
                <hr />
                <div className="d-flex justify-content-end">
                  <Button variant="outline-success" onClick={() => window.location.replace("/")} onClick={this.props.onHide}>Закрыть</Button>
                </div>
            </Alert>

            { this.props.showSelectDate ? <br/> : null }
            <Alert variant="danger" dismissible show={this.props.showError} onClose={this.props.closeError} >
              <Alert.Heading>Места не найдены!</Alert.Heading>
                <p>
                  К сожалению, по Вашему запросу не было найдено мест.
                  Попробуйте изменить параметры.
                </p>
            </Alert>

          </Modal.Body>
          <Modal.Footer>
            <Button 
            onClick={this.props.checkAvailability}
            disabled={this.props.checkAvailabilityButtonDisabled} 
            >
              { this.props.processingRequest ?
                <div> 
                  <Spinner
                  as="span"
                  animation="grow"
                  size="sm"
                  role="status"
                  aria-hidden="true"
                  />
                  Загрузка...
                </div> 
              : <div>Проверить наличие мест</div> 
              }
            </Button>
          </Modal.Footer>

      </Modal>    
    );
  }
}