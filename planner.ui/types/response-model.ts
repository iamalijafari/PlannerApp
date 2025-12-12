import { MessageKey } from '@/types/message-key';

export interface ResponseModel<T> {
    success: boolean;
    result: T;
    messageKey: MessageKey;
}