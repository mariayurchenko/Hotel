import React, { Component } from 'react';

export class SelectDate extends Component {

    constructor(props) {
      super(props);

      var date;
      if(props.dateStart){
          date = new Date(props.dateStart);
      }else{
        date = new Date();
      }
      date.setDate(date.getDate() + 1);
      date = date.toISOString().slice(0, 10);
      
      var today = new Date().toISOString().slice(0, 10);

      this.state = {
          dateStart: props.dateStart,
          dateEnd: props.dateEnd,
          minDateStart: today,
          minDateEnd: date
      };      

      this.findApartments = this.findApartments.bind(this);
      this.changeDateStart = this.changeDateStart.bind(this);
  }

  changeDateStart(event) {
      this.setState({ dateStart: event.target.value });

      var dateEnd = document.getElementById("dateEnd");
      if (new Date(dateEnd.value) <= new Date(event.target.value)) {
          dateEnd.value = null;
          this.setState({ dateStart: event.target.value, dateEnd: null });
      } else {
          this.setState({ dateStart: event.target.value });
      }

      var date = new Date(event.target.value);
      date.setDate(date.getDate() + 1);
      dateEnd.setAttribute("min", date.toISOString().slice(0, 10));
  }

  findApartments(event) {
      event.preventDefault();
      if (this.state.dateEnd && this.state.dateStart && new Date(this.state.dateEnd) > new Date(this.state.dateStart)) {
          window.location.replace(`/apartments?dateStart=${this.state.dateStart}&dateEnd=${this.state.dateEnd}`);
      }
  }

  render() {
    return (
        <div>
            <form onSubmit={this.findApartments} onChange={this.props.onChangeSearchDataForm}>

                <label>
                    <span>Дата заезда: </span>
                    <input id='dateStart' ref={this.props.dateStartRef} type='date' min={this.state.minDateStart} defaultValue= {this.state.dateStart} onChange={this.changeDateStart} required />
                </label>

                <label>
                    <span>Дата отъезда: </span>
                    <input id='dateEnd' ref={this.props.dateEndRef} type='date' min={this.state.minDateEnd} defaultValue= {this.state.dateEnd} onChange={event => this.setState({ dateEnd: event.target.value })} required />
                </label> 

                <label>
                    <input type='submit' disabled={!this.state.dateStart || !this.state.dateEnd} value='Найти' />
                </label> 

        </form>
      </div>
    );
  }
}