import { render, screen, fireEvent } from '@testing-library/react'
import '@testing-library/jest-dom'
import { Button } from '../../../../components/common/button';
//import { Button } from '~/src/components/common/button';


test("Button_Renders_Successfully", () => {
    render(<Button buttonType={{ type: "Solid" }} text="Button1" onClick={() => {}} />);

    const element = screen.getByText(/Button/i);

    expect(element).toBeInTheDocument();
});

test('Button-Rendersrenders button with correct label', () => {
    const { getByText } = render(<Button buttonType={{ type: "Solid" }} text="Button1" onClick={() => {}} />);
    const buttonElement = getByText('Click me');
    expect(buttonElement).toBeInTheDocument();
  });

test('calls onClick handler when button is clicked', () => {
    const onClickMock = jest.fn();
    const { getByTestId } = render(<Button buttonType={{ type: "Solid" }} text="Button1" onClick={onClickMock} />);
    const buttonElement = getByTestId('custom-button');
    
    fireEvent.click(buttonElement);
  
    expect(onClickMock).toHaveBeenCalled();
  });