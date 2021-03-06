import {
  LOADING_EDIT_TOPIC, EDIT_TOPIC_FAILED, EDIT_TOPIC_SUCCEEDED, EDIT_TOPIC_INACTIVE,
} from '../../constants/EditTopicStatus';
import {
  EDIT_TOPIC_START, EDIT_TOPIC_SUCCESS, EDIT_TOPIC_FAIL, SUSPEND_EDIT_TOPIC,
} from '../actions/types/editTopic';

const initialState = {
  status: '',
};

const editTopic = (state = initialState, action) => {
  switch (action.type) {
    case EDIT_TOPIC_START:
      return {
        status: LOADING_EDIT_TOPIC,
      };
    case EDIT_TOPIC_FAIL:
      return {
        status: EDIT_TOPIC_FAILED,
      };
    case EDIT_TOPIC_SUCCESS:
      return {
        status: EDIT_TOPIC_SUCCEEDED,
      };
    case SUSPEND_EDIT_TOPIC:
      return {
        status: EDIT_TOPIC_INACTIVE,
      };
    default:
      return state;
  }
};

export default editTopic;
