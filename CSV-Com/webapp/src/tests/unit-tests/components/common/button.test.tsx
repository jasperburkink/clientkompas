import { render, screen, fireEvent } from '@testing-library/react'
import '@testing-library/jest-dom'
import { Button } from 'components/common/button';


test("Renders_MainFlow_RendersSuccessfully", () => {
    // Arrange
    render(<Button buttonType={{ type: "Solid" }} text="Button1" onClick={() => {}} />);

    // Act
    const element = screen.getByText(/Button/i);

    // Asser
    expect(element).toBeInTheDocument();
});

test('Renders_TextOnButton_HasCorrectText', () => {
    // Arrange
    var buttonText = "Text on the button";

    // Act
    const { getByText } = render(<Button buttonType={{ type: "Solid" }} text={buttonText} onClick={() => {}} />);
    const buttonElement = getByText(buttonText);

    // Assert
    expect(buttonElement).toBeInTheDocument();
  });

test('OnClickEvent_FireOnClick_EventHasBeenFired', () => {
    // Arrange
    const onClickMock = jest.fn();

    // Act
    const { getByTestId } = render(<Button buttonType={{ type: "Solid" }} text="Button1" onClick={onClickMock} />);
    const buttonElement = getByTestId('button_test');    
    fireEvent.click(buttonElement);

    // Assert
    expect(onClickMock).toHaveBeenCalled();
  });