import axios from 'axios';

export const getApartment = async (apartmentId) => {
    const res = await axios.get(`/api/apartment/${apartmentId}`);
    return res.data;
};

export const getApartments = async (params?) => {
    const res = await axios.get(`/api/apartment`, { params });
    return res.data;
};

export const getPriceIfApartmentTypeAvailable = async (params) => {
    const res = await axios.get(`/api/apartment/price`, { params });
    return res.data;
}