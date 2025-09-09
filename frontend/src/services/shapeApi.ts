import axios from "axios";
import {ParseShapeRequest, ParseShapeResponse} from "../types/shapes";

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5010';

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    },
    timeout: 10000
});

export const parseShape = async (command: string) => {
    try {
        const request: ParseShapeRequest = {command: command};

        const response = await apiClient.post<ParseShapeResponse>('/shape/parse', request);

        return response.data as ParseShapeResponse;
    } catch (error) {
        if (axios.isAxiosError(error)) {
            if (error.response) {
                return error.response.data as ParseShapeResponse;
            } else if (error.request) {
                return {success: false, errorMessages: 'Network error occurred. Please try again later.', shape: null};
            } else {
                return {success: false, errorMessages: 'Error making the request'};
            }
        }
        return {success: false, errorMessages: 'An unexpected error occurred'};
    }
};