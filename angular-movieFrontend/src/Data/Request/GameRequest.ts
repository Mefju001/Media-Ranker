export interface GameRequest {
    title: string;
    description: string;
    genre: {
        name: string;
    };
    releaseDate: string;
    language: string;
    gameStatus: string;
    developer: string;
    engine: string;
    pegiRating: number;
    platforms: string[];
    supportsCrossPlay: boolean;
}