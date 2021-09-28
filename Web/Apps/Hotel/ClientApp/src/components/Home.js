import React, { Component } from 'react';
import { SelectDate } from './SelectDate';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <SelectDate />
            </div>
        );
    }
}