import axios from 'axios';

export interface MeteoriteSummary {
  year: number;
  count: number;
  totalMass: number;
}

export interface FilterValueRequest {
  yearFrom?: string;
  yearTo?: string;
  recClass?: string;
  name?: string;
  sortField?: string;
  sortOrder?: 'asc' | 'desc';
}

export const http = {
  getClasses: () =>
    axios.get<string[]>('api/meteorites/classes').then((x) => x.data),
  getData: (filterValue: FilterValueRequest) =>
    axios.get('api/meteorites', { params: filterValue }).then((x) => x.data),
};
