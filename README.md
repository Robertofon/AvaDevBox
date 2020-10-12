# AvaDevBox
Controls for AvaloniaUI developers

![.NET Core build AvaDevBox](https://github.com/flinkebits/AvaDevBox/workflows/.NET%20Core%20build%20AvaDevBox/badge.svg)

This is a currently emerging project. It shall and already does provide a brilliant collection of lookless [AvaloniaUI](http://avaloniaui.net/) controls.
These can be used by GUI developers in GUI applications based on [AvaloniaUI](http://avaloniaui.net/). 

## License
See [License.md](LICENSE).

## Design goals

Goals are:
* Having i18n in mind
* Keyboard navigation
* Easy and intuitive to use
* Primary lookless design
* Well made architecture
* Well documented
* All the necessities on  board
* Runs on all AvaloniaUI platforms

## Current status
Instable

Do not yet rely on it. It is work in progress. Even AvaloniaUI still does breaking changes from release
to release. So everything is in flow. But feel free to use some and adhere to the license by mentioning
the author.

## Releases

No releases yet. Planned.

## Examples of contained controls

* MenuButton
* RatingControl
* Star shape
* LedIndicator
* LedButton
* RangeSlider
* WheelSlider

### Detail:

* MenuButton is a control that allows a popout with additional Controls. 
  ![](img/MenuButton.png)

* RatingControl allows to give a rating between 0.0 and 1.0 on a varying number of stars
  or other shape (if templated differently)
  ![](img/RatingControl.png)

* StarShape is a waste product of RatingControl and may be used anywhere.
  ![](img/StarShape.png)

* LedButton and LedIndicator are visualized LEDs in configurable colors either as
  shape or as checkbox control (including three-state) 
  ![](img/LedButton.png)

* RulerShape is a shape which allows a wide customization to show different styles of rulers
  it supports three different tick legnths and can hence be configured from a straight row of dashes
  to inch rulers with quarters and eithghts to centimeter rulers with 1.0, 0.5 and 0.1 dashes.
  ![](img/RulerShape.png)

* WheelSlider: This is a kind of slider. In fact it looks like a wheel inset into
  the surface. The user can just move the wheel and thus change the value.
  It supports wrap around and a speed up factor.
  ![](img/WheelSlider.png)

* Future
    * RangeSlider
    * BusyIndicator
    * TagInput  (aka Chips)
    * SevenSegmentDisplay
    * RotationSlider
    * NotificationBar
    * Gnatt diagram
    * Flow chart area
    * Flex layout tree
    * Tree list control


# We do even have some newer Panels included:

* EquiSpreadWrapPanel

![](img/EquiSpreadPanel.png)
This is primarily a wrap panel WrapPanel that holds an arbitrary number
of childs. The difference to WrapPanel is that it spreads its childs to the whole width
(if orientation is Horizontal) or height respectively. That for every line individually.
This happens if ItemWidth or ItemHeight are set to double.NaN.
If you set an  ItemWidth or ItemHeight, though, all items will have that
height/width applied. Since items are still spread equally about the space (but in the last line),
this is the right panel to use, if you want to make a gallery like listing.

# And of course some utilities

like 

* DockEnum 
    This allows to use enums in XAML as a markup extension:

    ![](img/DockEnum.png)

        Seealso: ![Roblog](https://log.koepferl.de/2020/09/20/mit-avaloniaui-enums-in-xaml-zeigen/)

