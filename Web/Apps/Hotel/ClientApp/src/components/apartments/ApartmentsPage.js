import React, { Component } from 'react';
import queryString from 'query-string';

//components
import { ApartmentsList } from './ApartmentsList';
import { SelectDate } from '../SelectDate';

export class ApartmentsPage extends Component {

    constructor(props) {
        super(props);
        var params = queryString.parse(this.props.location.search);
        this.state = {
            dateStart: params.dateStart,
            dateEnd: params.dateEnd
        }
    }

    render() {
        return (
            <div>

                <SelectDate dateStart={this.state.dateStart} dateEnd={this.state.dateEnd} />
                <ApartmentsList dateStart={this.state.dateStart} dateEnd={this.state.dateEnd} /> 

            </div> 
        );
    }
}