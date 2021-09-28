import React, { Component } from 'react';
import {Button, ButtonToolbar} from 'react-bootstrap'
import InputSpinner from 'react-bootstrap-input-spinner' 

export class BookingParameters extends Component {

    constructor(props) {
      super(props);
      this.state = {
          adults: props.adults ?? 1,
          children: props.children ?? 0,
      };
  }

  render() {

    function None(){
        return(
          <div class="row">
            <label style={{textAlign: "center"}}>---</label>
          </div>
        )
      }

    return (
        <div>
            <table class="table table-striped">
                <thead class="thead-dark" style={{fontSize:"24px", textAlign: "center"}}>
                    <tr>
                        <th>Дата заезда</th>
                        <th>Дата отъезда</th>
                        <th>Гости</th>
                    </tr>
                </thead>
                <tbody style={{fontSize:"22px"}}>
                    <tr>
                        <td>{ this.props.dateStart && this.props.dateEnd ? <div> 
                                <div class="row"><label style={{textAlign: "center"}}>{this.props.dateStart}</label></div>
                                <div class="row"><label style={{textAlign: "center"}}>c 12:00</label></div>
                            </div> : <None/> }
                        </td>
                        <td>{ this.props.dateStart && this.props.dateEnd ? <div> 
                                <div class="row"><label style={{textAlign: "center"}}>{this.props.dateEnd}</label></div>
                                <div class="row"><label style={{textAlign: "center"}}>{this.props.night}</label></div>
                            </div> : <None/> }
                        </td>
                        <td width={"30%"}>
                            <div class="row">
                                <div class="col-8">Взрослых</div>
                                <div class="col-4">
                                    <InputSpinner
                                    type={'real'}
                                    precision={2}
                                    max={5}
                                    min={1}
                                    step={1}
                                    value={this.state.adults}
                                    onChange={this.props.onAdultsChange}
                                    size="xs"
                                    />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-8">Детей</div>
                                <div class="col-4">
                                    <InputSpinner
                                    type={'real'}
                                    precision={2}
                                    max={5}
                                    min={0}
                                    step={1}
                                    value={this.state.children}
                                    onChange={this.props.onChildrenChange}
                                    size="xs"
                                    />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr> 
                        <td colSpan="3">
                            <div class="row">
                                <div  class="col-8">
                                <div class="d-flex justify-content-end">
                                    { this.props.price ? <label style={{textAlign: "right"}}>Цена: {this.props.price}</label> : null }
                                </div>
                                </div>
                                <div class="col-4">
                                <div class="d-flex justify-content-end">
                                    <ButtonToolbar>
                                        <Button variant='primary' onClick={this.props.changeParameters}>Изменить параметры поиска</Button>
                                    </ButtonToolbar>
                                </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
      </div>
    );
  }
}