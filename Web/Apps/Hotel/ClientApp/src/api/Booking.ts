import axios from 'axios';

export const createBooking = async (booking) => {
    const res = await axios.post(`/api/booking`, booking);
    return res.data;
}