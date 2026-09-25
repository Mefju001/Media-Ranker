import { MediaResponse } from "./MediaResponse";
import { UserDetailsResponse } from "./UserDetailsResponse";

export interface UserInteractionsResponse {
    id: string;
    user:UserDetailsResponse;
    MediaResponse: MediaResponse;
    LikedDate: Date;
}