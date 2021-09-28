import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ApartmentPage } from './components/apartments/ApartmentPage';
import { ApartmentsPage } from './components/apartments/ApartmentsPage';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route exact path="/apartments" component={ApartmentsPage} />
                <Route exact path="/apartment/:id" component={ApartmentPage} />
                <Route exact path="/booking/:id" component={ApartmentPage} />
            </Layout>
        );
    }
}