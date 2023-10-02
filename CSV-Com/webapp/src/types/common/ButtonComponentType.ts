export interface ButtonType {
    type: 'Solid' | 'NotSolid' | 'Underline'
}

export function getClassNameButtonType(param: string) {
    switch(param)
    {
      default:
      case 'Solid':
        return 'button button-solid';
      case 'NotSolid':
        return 'button button-not-solid';
      case 'Underline':
        return 'button button-underline';
    }
  };