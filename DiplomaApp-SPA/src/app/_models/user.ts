import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    // ? - optional properties; need to come after required properties
    interests?: string;
    introduction?: string;
    photos?: Photo[];
}
