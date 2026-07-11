export interface MovieRequest {
    title: string;
    description: string;
    genre: {
        name: string;
    };
    releaseDate: string;
    language: string;
    director: {
        name: string;
        surname: string;
    };
    duration: string;
    distributionType: string;
    status: string;
}