import { ERatingVote } from "../../ClientPages/DetailsPages/Data/ERatingVote";
import { ETypeInteractions } from "../../ClientPages/DetailsPages/Data/ETypeInteractions";

export interface UserInteractionsRequest {
    mediaId: string, 
    typeInteractions?: ETypeInteractions|null, 
    ratingVote?: ERatingVote|null
}