import { Language } from "./language";
import { MessageKey } from "./message-key";

export interface TranslationRequestModel {
    MessageKey: MessageKey;
    Language: Language;
}